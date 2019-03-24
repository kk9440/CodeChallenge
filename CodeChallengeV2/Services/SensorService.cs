using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class SensorService : ISensorService
    {
        private static byte[] StringToByteArray(string hex) {
            byte[] bytes = new byte[(hex.Length >> 1)];
            for (int idx = 0; idx < hex.Length; idx += 2) bytes[(idx >> 1)] = Convert.ToByte(hex.Substring(idx, 2), 16);
            return bytes;
        }

        private static T GetData<T>(byte[] bytes, int offset, int dataSize) {
            T result = default(T);
            for (int i = 0; i < Marshal.SizeOf(default(T)); i++) {
                result = (result * (dynamic)256) | bytes[0];
            }

            return result;
        }

        public Task<SensorPayloadDecoded> DecodePayload(SensorPayload payload)
        {
            byte[] bytes = StringToByteArray(payload.FPort.ToString("X2") + payload.Data);
            int arrayIdx = 0;
            var res = new SensorPayloadDecoded()
            {
                DevEUI = payload.DevEUI,
                Time = payload.Time            
            };

            while (arrayIdx < bytes.Length) {
                double data = (double)((Int16)(bytes[arrayIdx+2] << 8 | bytes[arrayIdx+1]));
                switch(bytes[arrayIdx]) {
                    case 20:
                        arrayIdx += 3;                     
                        break;
                    case 40:
                        res.TempInternal = data / 100.0;
                        arrayIdx += 3;                     
                        break;
                    case 41:
                        res.TempRed = data / 100.0;
                        arrayIdx += 3;                     
                        break;
                    case 42:
                        res.TempBlue = data / 100.0;                
                        arrayIdx += 3;                     
                        break;
                    case 43:
                        res.TempHumidity = data / 100.0;
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
