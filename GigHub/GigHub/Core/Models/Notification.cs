﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        [Required]
        public Gig Gig { get; private set; }

        protected Notification()
        {
        }

        private Notification(NotificationType type, Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException(nameof(gig));

            Type = type;
            Gig = gig;
            DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue)
        {
            return new Notification(NotificationType.GigUpdated, newGig)
            {
                OriginalDateTime = originalDateTime,
                OriginalVenue = originalVenue
            };
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }
    }
}