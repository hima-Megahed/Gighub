﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.Repositories
{
    public class FollowingRepository
    {
        private readonly ApplicationDbContext _context;
        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Following> GetFollowing(string userId, string artistId)
        {
            return _context.Followings
                .Where(f => f.FolloweeId == artistId && f.FollowerId == userId);
        }
    }
}