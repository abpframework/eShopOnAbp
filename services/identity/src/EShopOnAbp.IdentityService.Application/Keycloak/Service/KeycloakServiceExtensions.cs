using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;

namespace EShopOnAbp.IdentityService.Keycloak.Service;

/* Extensions to create unique strings based on list values */
public static class KeycloakServiceExtensions
{
    public static string GenerateCacheKeyBasedOnValues(this IEnumerable<Role> roles)
    {
        return GenerateUniqueCacheKeyBasedOnList(roles);
    }

    public static string GenerateCacheKeyBasedOnValues(this IEnumerable<User> users)
    {
        return GenerateUniqueCacheKeyBasedOnList(users);
    }

    private static string GenerateUniqueCacheKeyBasedOnList<T>(IEnumerable<T> list)
    {
        string serializedList = JsonSerializer.Serialize(list);
        byte[] bytes = Encoding.UTF8.GetBytes(serializedList);
        byte[] hash = SHA256.Create().ComputeHash(bytes);
        string hashString = BitConverter.ToString(hash).Replace("-", "");
        return hashString;
    }
}