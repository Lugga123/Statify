﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;
using StatifyProject.Application.Infrastructure;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UserController : ControllerBase
{
    // DTO class for the JSON body of the login request
    public record CredentialsDto(string username, string password);

    private readonly StatifyContext _db;
    private readonly IConfiguration _config;  // Needed to read the secret from appsettings.json
    public UserController(StatifyContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    /// <summary>
    /// POST /api/user/login
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] CredentialsDto credentials)
    {
        // Read the secret from appsettings.json via IConfiguration
        // This is NOT the salt of the user password! It is the key to sign the JWT, so
        // the client cannot manupulate our token.
        var secret = Convert.FromBase64String(_config["Secret"]);
        var lifetime = TimeSpan.FromHours(3);
        // User exists in our database and the calculated hash matches
        // the password hash in the database?
        var user = _db.Users.FirstOrDefault(a => a.Username == credentials.username);
        if (user is null) { return Unauthorized(); }
        if (!user.CheckPassword(credentials.password)) { return Unauthorized(); }

        string role = "Admin";  // TODO: Set your role based on your rules.
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Payload for our JWT.
            Subject = new ClaimsIdentity(new Claim[]
            {
                // Write username to the claim (the "data zone" of the JWT).
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                // Write the role to the claim (optional)
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            }),
            Expires = DateTime.UtcNow + lifetime,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Return the token so the client can save this to send a bearer token in the
        // subsequent requests.
        return Ok(new
        {
            user.Username,
            UserGuid = user.Guid,
            Role = role,
            Token = tokenHandler.WriteToken(token)
        });
    }

    /// <summary>
    /// GET /api/user/me
    /// Gets information about the current (authenticated) user.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetUserdata()
    {
        // No username is set in HttpContext? Should never occur because we added the
        // Authorize annotation. But the properties are nullable, so we have to check.
        var username = HttpContext?.User.Identity?.Name;
        if (username is null) { return Unauthorized(); }

        // Valid token, but no user match in the database (maybe deleted by an admin).
        var user = _db.Users.FirstOrDefault(a => a.Username == username);
        if (user is null) { return Unauthorized(); }
        return Ok(new
        {
            user.Username,
            user.Email,
            FavoriteArtistGuid = user.FavoriteArtist?.Guid,
            FavoriteArtistName = user.FavoriteArtist?.Name,
            FavoriteSongGuid = user.FavoriteArtist?.Guid,
            FavoriteSongTitle = user.FavoriteSong?.Title
        });
    }

    /// <summary>
    /// GET /api/user/all
    /// List all users.
    /// Only for users which has the role admin in the claim of the JWT.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        var user = _db.Users
            .Select(a => new
            {
                a.Username,
                a.Email
            })
            .ToList();
        if (user is null) { return BadRequest(); }
        return Ok(user);
    }

    [HttpPut("setSong/{guid:Guid}")]
    public IActionResult SetFavoriteSong(Guid guid)
    {
        var username = HttpContext?.User.Identity?.Name;
        if (username is null) { return Unauthorized(); }
        var user = _db.Users.FirstOrDefault(a => a.Username == username);
        if (user is null) { return Unauthorized(); }
        var song = _db.Songs.FirstOrDefault(s => s.Guid == guid);
        if (song is null) { return NotFound(); }
        user.FavoriteSong = song;
        _db.SaveChanges();
        return NoContent();
    }
    [HttpPut("setArtist/{guid:Guid}")]
    public IActionResult SetFavoriteArtist(Guid guid)
    {
        var username = HttpContext?.User.Identity?.Name;
        if (username is null) { return Unauthorized(); }
        var user = _db.Users.FirstOrDefault(a => a.Username == username);
        if (user is null) { return Unauthorized(); }
        var artist = _db.Artists.FirstOrDefault(a => a.Guid == guid);
        if (artist is null) { return NotFound(); }
        user.FavoriteArtist = artist;
        _db.SaveChanges();
        return NoContent();
    }
}