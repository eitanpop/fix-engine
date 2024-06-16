using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application
{
    public class Sqs : IQueueReader, IQueueWriter
    {
        private readonly IAmazonSQS _client;
        public Sqs(IAmazonSQS client)
        {
            _client = client;
        }
        public async Task<IList<string>?> ReadAsync(bool deleteAfterRead)
        {
            var response = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20,
            });
      
            var messages = response.Messages;

            if (deleteAfterRead && messages is { Count: > 0 })
            {
                await _client.DeleteMessageBatchAsync(new DeleteMessageBatchRequest
                {
                    Entries = response.Messages
                        ?.Select(x => new DeleteMessageBatchRequestEntry(x.MessageId, x.ReceiptHandle)).ToList()
                });
            }

            return messages?.Select(x => x.Body).ToList();
        }

        public async Task<bool> WriteAsync(string message)
        {
            var response = await _client.SendMessageAsync(new SendMessageRequest
            {
                MessageBody = message
            });
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
