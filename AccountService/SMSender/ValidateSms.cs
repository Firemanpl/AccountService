using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PhoneNumbers;

namespace AccountService.SMSender
{
    public interface IValidateSms
    {
        Task<bool> ValidateAndSendSms(SmsDto dto, CancellationToken cancellationToken);
    }

    public class ValidateSms : IValidateSms
    {
        private readonly string[] _ports;
        private readonly SerialPort _serialPort;
        public ValidateSms()
        {
            var serialPort = new SerialPort();
            var ports = SerialPort.GetPortNames();
            Console.WriteLine("Serial Ports: ");
            for (int i=0;i<ports.Length; i++)
            {
                Console.WriteLine(i + 1 + ". " + ports[i]);
            }
            var exist = ports.Contains("/dev/ttyUSB0");
            if (!exist) throw new NotImplementedException("Modem 3G/4G is not plugged into usb.");
            
        }
        public async Task<bool> ValidateAndSendSms(SmsDto dto, CancellationToken cancellationToken)
        {
            try
            {
                PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
                PhoneNumber isPhoneValid = phoneNumberUtil.Parse(dto.PhoneNumber,dto.Nationality);
                string formattedPhone = phoneNumberUtil.Format(isPhoneValid, PhoneNumberFormat.E164);
                // Console.WriteLine($"Phone is valid.{formattedPhone}");
                // Console.WriteLine($"Verify Code is: {dto.VerificationCode}");
                dto.PhoneNumber = formattedPhone;
            }
            catch (Exception)
            {
                Console.WriteLine("Phone is not valid.");
                return false;
            }
            if (dto.VerificationCode.Length > 160)
            {
                return false;
            }

            await SendSms(dto, cancellationToken);
            return true;
        }

        private async Task SendSms(SmsDto dto, CancellationToken cancellationToken )
        {
            _serialPort.PortName = _ports.LastOrDefault();
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
            if (_serialPort.IsOpen)
            {
                resend:
                _serialPort.WriteLine("AT+CMGF = 1");
                await Task.Delay(3000,cancellationToken);
                _serialPort.Write("AT+CMGS=\"" + dto.PhoneNumber + "\"\r\n");
                await Task.Delay(1000,cancellationToken);
                _serialPort.Write("Your verification code: " + dto.VerificationCode.Insert(4,"-") + "\x1A");
                string result = _serialPort.ReadExisting();
                Console.WriteLine(result);
                if (result.Contains("ERROR"))
                {
                    goto resend;
                }
                _serialPort.Close();
            }
        }
    }
}