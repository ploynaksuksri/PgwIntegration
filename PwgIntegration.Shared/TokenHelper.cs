using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace PwgIntegration.Shared
{
    public class TokenHelper
    {
        private readonly string _secretKey;
        private readonly IJwtEncoder _encoder;
        private readonly IJwtDecoder _decoder;

        public TokenHelper(string secretKey)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtValidator validator = new JwtValidator(serializer, new UtcDateTimeProvider());
            _encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            _decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            _secretKey = secretKey;
        }

        public string Encode(object payload)
        {
            return _encoder.Encode(payload, _secretKey);
        }

        public string Decode(string token)
        {
            return _decoder.Decode(token, _secretKey, verify: true);
        }
    }
}