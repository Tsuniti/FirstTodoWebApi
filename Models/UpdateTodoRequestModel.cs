﻿namespace FirstTodoWebApi.Models;

public class UpdateTodoRequestModel
{
    public Guid TodoId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}