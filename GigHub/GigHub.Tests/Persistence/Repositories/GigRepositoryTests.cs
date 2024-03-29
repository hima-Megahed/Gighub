﻿using System;
using System.Collections;
using System.Data.Entity;
using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _gigRepository;
        private Mock<DbSet<Gig>> _mockGigs;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGigs = new Mock<DbSet<Gig>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);

            _gigRepository = new GigRepository(mockContext.Object);

        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig(){DateTime = DateTime.Now.AddDays(-1), ArtistId = "1"};

            _mockGigs.SetSource(new[] {gig});

            var gigs = _gigRepository.GetUpcomingGigsByArtist("1");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCanceled_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepository.GetUpcomingGigsByArtist("1");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForADifferentArtist_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepository.GetUpcomingGigsByArtist("1-");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForTheGivenArtistAndIsInFuture_ShouldBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _gigRepository.GetUpcomingGigsByArtist("1");

            gigs.Should().Contain(gig);
        }
    }
}
