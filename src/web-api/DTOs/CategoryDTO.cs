using System;

namespace api.DTOs;

public class CategoryDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
}
