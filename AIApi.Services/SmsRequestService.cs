using System;
using System.Collections.Generic;

namespace AIApi.Services
{
    public class SmsRequestService : ISmsRequestService
    {
        private readonly HashSet<Guid> _smsIds = new();

        public void AddSmsId(Guid smsId)
        {
            _smsIds.Add(smsId);
        }

        public bool GetSmsId(Guid smsId)
        {
            return _smsIds.Contains(smsId);
        }

        public void RemoveSmsId(Guid smsId)
        {
            _smsIds.Remove(smsId);
        }
    }
}
