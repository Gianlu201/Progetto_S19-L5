﻿namespace Progetto_S19_L5.Settings
{
    public class Jwt
    {
        public required string SecurityKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required int ExpiresInMinutes { get; set; }
    }
}
