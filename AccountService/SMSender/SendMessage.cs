using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountService.SendSMS;
using Microsoft.Extensions.Hosting;

namespace AccountService.SMSender
{
    public interface ISendMessage
    {
        bool AddSmsToQueue(SmsDto dto);
    }

    public class SendMessage : BackgroundService, ISendMessage
    {
        private readonly string[] _ports;
        private readonly Queue<SmsDto> _queue = new();
        private readonly SerialPort _serialPort;
        public bool AddSmsToQueue(SmsDto dto)
        {
            if (dto.VerificationCode.Length > 160)
            {
                return false;
            }
            _queue.Enqueue(dto);
            //Console.WriteLine(_queue.Count);
            return true;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serialPort = new SerialPort();
            var ports = SerialPort.GetPortNames();
            Console.WriteLine("Serial Ports: ");
            for (int i=0;i<ports.Length; i++)
            {
                Console.WriteLine(i + 1 + ". " + ports[i]);
            }
            var exist = ports.Contains("/dev/ttyUSB0");
            if (!exist) throw new WarningException("Modem 3G/4G is not plugged into usb.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_queue.Count > 0)
                    {
                        var dto = _queue.Dequeue();

                        _serialPort.PortName = _ports.LastOrDefault();
                        _serialPort.BaudRate = 9600;
                        _serialPort.ReadTimeout = 500;
                        _serialPort.WriteTimeout = 500;
                        _serialPort.Open();
                        if (_serialPort.IsOpen)
                        {
                            _serialPort.WriteLine("AT+CMGF = 1");
                            await Task.Delay(3000, stoppingToken);
                            _serialPort.Write("AT+CMGS=\"" + dto.PhoneNumber + "\"\r\n");
                            await Task.Delay(1000, stoppingToken);
                            _serialPort.Write("Your verification code: " + dto.VerificationCode.Insert(4,"-") + "\x1A");
                            Console.WriteLine($"Sms sent to: {dto.PhoneNumber}");
                            _serialPort.Close();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
    }
}