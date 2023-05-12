﻿using Microsoft.Extensions.DependencyInjection;
using StatifyProject.Application.Infrastructure;
using StatifyProject.Application.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StatifyProject.Application.Dto
{
    public record UserDto(
     Guid Guid,

     [StringLength(20, MinimumLength = 2, ErrorMessage = "Länge des Usernames ist ungültig.")]
    string Username,

     [StringLength(50, MinimumLength = 3, ErrorMessage = "Die Länge der Email ist ungültig.")]
    string Email,

     string AccessToken,

     string RefreshToken,

     [StringLength(255, MinimumLength = 6, ErrorMessage = "Die Länge des Passworts ist ungültig.")]
    string Password,

     [StringLength(255, MinimumLength = 1, ErrorMessage = "Die Länge der Bio ist ungültig.")]
    string Bio,

     string? FavoriteSong,

     string? FavoriteArtist,

     UserRole Role
 );

    //: IValidatableObject




    /*
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var db = validationContext.GetRequiredService<StatifyContext>();
        if (!db.Songs.Any(a => a.Guid == FavoriteSongGuid))
        {
            yield return new ValidationResult("Song does not exist", new[] { nameof(FavoriteSongGuid) });
        }
        if (!db.Artists.Any(c => c.Guid == FavoriteArtistGuid))
        {
            yield return new ValidationResult("Artist does not exist", new[] { nameof(FavoriteArtistGuid) });
        }
    }
*/
}


