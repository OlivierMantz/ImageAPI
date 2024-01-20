using ImageAPI.Models;

namespace ImageAPI.Service
{
    public interface IMessagePublisher
    {
        void Publish(NewPostMessage message);

    }
}
