using System;

namespace Chaldea.Services.Users.Dto
{
    public class UserDetailDto
    {
        public long Favorites { get; set; }

        public long Achievements { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Signature { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public string Id { get; set; }
    }
}