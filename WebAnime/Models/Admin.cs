﻿using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class Admin
{
    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int Id { get; set; }
}
