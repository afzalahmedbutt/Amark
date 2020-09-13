using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core;
using TradingPortal.Core.Domain;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Business
{
    public class TopicManager : ITopicManager
    {
        IRepository<Topic> _topicRepository;
        public TopicManager(IRepository<Topic> topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<TopicViewModel> GetTopicBySystemName(string systemName)
        {
            if (String.IsNullOrEmpty(systemName))
                return null;
            var topic = await _topicRepository.FindAsync(t => t.SystemName == systemName);
            if (topic == null)
                return null;

            var model = new TopicViewModel()
            {
                
                SystemName = topic.SystemName,
                IncludeInSitemap = topic.IncludeInSitemap,
                IsPasswordProtected = topic.IsPasswordProtected,
                Title = topic.Title,
                Body = topic.Body
            };
            return model;
            //query = query.Where(t => t.SystemName == systemName);
        }
    }
}
