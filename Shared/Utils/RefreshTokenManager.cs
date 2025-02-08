using System.Collections.Concurrent;
using SisandBackend.Entities.Users;

namespace SisandBackend.Shared.Utils
{
    public static class RefreshTokenManager
    {
        #region Properties
        private static readonly ConcurrentDictionary<string, User> _refreshTokens = new();
        #endregion

        #region Methods
        public static void AddOrUpdateRefreshToken(string refreshToken, User user) => _refreshTokens.AddOrUpdate(refreshToken, user, (key, existingUser) => user);

        public static bool TryGetRefreshToken(string refreshToken, out User user) => _refreshTokens.TryGetValue(refreshToken, out user);
        
        public static bool RemoveRefreshToken(string refreshToken) => _refreshTokens.TryRemove(refreshToken, out _);
        #endregion
    }
}
