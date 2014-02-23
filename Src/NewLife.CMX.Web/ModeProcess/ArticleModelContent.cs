﻿using System;
using System.Collections.Generic;
using NewLife.CMX.Config;
using NewLife.CMX.TemplateEngine;
using XCode;

namespace NewLife.CMX.Web
{
    public class ArticleModelContent : ModelContentBase
    {
        public override string Process()
        {
            try
            {
                ArticleProvider.CurrentChannel = ChannelID;

                var article = Article.FindByID(ID);
                //获取一次内容，缓存文章内容
                var cont = article.Content;
                if (article == null) return "不存在该记录！";

                LeftMenu = LeftMenuContent.GetContent(Channel, article.CategoryID);

                var dic = new Dictionary<String, String>();
                dic.Add("Address", Address);
                dic.Add("ID", ID.ToString());
                dic.Add("CategoryID", article.CategoryID.ToString());
                dic.Add("ChannelName", Channel.Name);
                //dic.Add("Suffix", Channel.Suffix);

                var engine = new CMXEngine(TemplateConfig.Current, WebSettingConfig.Current);
                engine.ArgDic = dic;
                //engine.Header = Header;
                //engine.Foot = Foot;
                //engine.LeftMenu = LeftMenu;
                engine.Suffix = Channel.Suffix;
                engine.Entity = article;
                engine.ModelShortName = Channel.Model.ShortName;

                return engine.Render(Address.EnsureEnd(".html"));
            }
            finally
            {
                ArticleProvider.CurrentChannel = 0;
            }
        }
    }
}