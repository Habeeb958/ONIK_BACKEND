﻿namespace ONIK_BANK.DTO
{
    public class Responses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class LogInResponse(bool Flag, string Token, string Message);
    }
}