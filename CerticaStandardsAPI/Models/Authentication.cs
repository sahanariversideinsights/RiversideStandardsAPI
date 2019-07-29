using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerticaStandardsAPI.Models
{
    public class Authentication 
    {
        private string _partnerId;  // the partnerid field
        public string partnerID    // the partnerid property
        {
            get
            {
                return _partnerId;
            }
            set
            {
                _partnerId = value;
            }
        }

        private string _partnerkey;  // the partnerkey field
        public string partnerKey    // the partnerkey property
        {
            get
            {
                return _partnerkey;
            }
            set
            {
                _partnerkey = value;
            }
        }
        private string _userId;  // the userId field
        public string userId    // the userId property
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        private string _expires;  // the expires field
        public string expires    // the expires property
        {
            get
            {
                return _expires;
            }
            set
            {
                _expires = value;
            }
        }

        private string _message;  // the message field
        public string message    // the message property
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }


        private string _keyBytes;  // the keyBytes field
        public string keyBytes    // the keyBytes property
        {
            get
            {
                return _keyBytes;
            }
            set
            {
                _keyBytes = value;
            }
        }

        private string _signature;  // the signature field
        public string signature    // the signature property
        {
            get
            {
                return _signature;
            }
            set
            {
                _signature = value;
            }
        }
      
    }
}