using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, 
                                    IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You Can't Send Message to Yourself");

            var sender = await _userRepository.GetUserByNameAsync(username);
            var recipient = await _userRepository.GetUserByNameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.Username,
                RecipientUsername = recipient.Username,
                Content = createMessageDto.Content,
            };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllAsync()) 
                return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to Send Message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery] 
            MessageParams messageparams)
        {
            messageparams.Username=User.GetUsername();
            var messages= await _messageRepository.GetMessagesForUser(messageparams);

            Response.AddPaginationHeader(messages.CurrentPage,messages.PageSize,
                messages.TotalCount,messages.TotalPages);

            return messages;            
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMessageThread(string username)
        {
            var currentUsername=User.GetUsername();

            return Ok(await _messageRepository.GetMessageThread(currentUsername,username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username=User.GetUsername();

            var message= await _messageRepository.GetMessage(id);

            if(message.Sender.Username != username && message.Recipient.Username != username)
            return Unauthorized();

            if(message.Sender.Username == username) message.SenderDeleted=true;

            if(message.Recipient.Username == username) message.RecipientDeleted=true;

            if(message.SenderDeleted && message.RecipientDeleted) 
                _messageRepository.DeleteMessage(message);

            if(await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("problem deleting the message");
        }
    }
}