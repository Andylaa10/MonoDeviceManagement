﻿namespace Application.DTOs;

public class PostDeviceDTO
{
    public string DeviceName { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; }
    public int? UserId { get; set; }
    public DateOnly? DateOfIssue { get; set; }
    public DateOnly? DateOfTurnIn { get; set; }
    public string RequestValue { get; set; }
}