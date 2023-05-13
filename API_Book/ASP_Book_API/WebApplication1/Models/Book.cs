using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public class Book
{
    public string Id { get; set; } = null!;


    public string? Name { get; set; }

   
    public double? Price { get; set; }


    public string? Author { get; set; }

    
    public string? DESCRIPTION { get; set; }

    
    public string? Sourceimg { get; set; }
}
