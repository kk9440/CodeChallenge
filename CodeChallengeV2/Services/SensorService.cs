using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class SensorService : ISensorService
    {
        private static byte[] StringToByteArray(byte firstByte, string hex) {
            byte[] bytes = new byte[(hex.Length >> 1) + 1];
            bytes[0] = firstByte;
            for (int idx = 0; idx < hex.Length; idx += 2) bytes[(idx >> 1) + 1] = Convert.ToByte(hex.Substring(idx, 2), 16);
            return bytes;
        }

        public Task<SensorPayloadDecoded> DecodePayload(SensorPayload payload)
        {
            byte[] bytes = StringToByteArray((byte)payload.FPort, payload.Data);
            int arrayIdx = 0;
            var res = new SensorPayloadDecoded()
            {
                DevEUI = payload.DevEUI,
                Time = payload.Time            
            };

            while (arrayIdx < bytes.Length) {
                switch(bytes[arrayIdx]) {
                    case 20:
                        arrayIdx += 3;                     
                        break;
                    case 40:
                        res.TempInternal = (double)((Int16)(bytes[arrayIdx+2] << 8 | bytes[arrayIdx+1])) / 100.0;
                        arrayIdx += 3;                     
                        break;
                    case 41:
                        res.TempRed = (double)((Int16)(bytes[arrayIdx+2] << 8 | bytes[arrayIdx+1])) / 100.0;
                        arrayIdx += 3;                     
                        break;
                    case 42:
                        res.TempBlue = (double)((Int16)(bytes[arrayIdx+2] << 8 | bytes[arrayIdx+1])) / 100.0;                
                        arrayIdx += 3;                     
                        break;
                    case 43:
                        res.TempHumidity = (double)((Int16)(bytes[arrayIdx+2] << 8 | bytes[arrayIdx+1])) / 100.0;
                        res.Humidity = (double)((SByte)(bytes[arrayIdx+3])) / 2.0;
                        arrayIdx += 4;                     
                        break;
                    default:
                        break;
                }
            }

            return Task.FromResult(res);
        }
    }
}
