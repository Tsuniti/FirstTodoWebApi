﻿namespace FirstTodoWebApi.Models;

public class DeleteTodoRequestModel
{
    public Guid TodoId { get; set; }
    public Guid UserId { get; set; }
}