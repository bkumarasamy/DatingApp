using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public string KnownUs { get; set; }
        public DateTime Created { get; set; }=DateTime.Now;
        public DateTime LastActive { get; set; }=DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
        public ICollection<PhotoDTO> Photos { get; set; }
    }
}