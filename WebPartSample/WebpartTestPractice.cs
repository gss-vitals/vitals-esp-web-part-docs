using GSS.Core.WebPart;
using GSS.Core.WebPart.DTO;
using GSS.Core.WebPart.Impl.KnowledgeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebpartTest
{
    public class WebpartTestPractice : BaseWebPart
    {
        //ConfigSettingList宣告
        private ConfigSettingList _configSettingList;
        public ConfigSettingList ConfigSettingList
        {
            set { _configSettingList = value; }
            get { return _configSettingList; }
        }

        public override WebPartHtml RenderContent(WebPartUserInfo actor, WebPartSettingList settingList)
        {
            return new WebPartHtml("<p>MyPractice</p>");
        }

        public override WebPartText RenderDescription(WebPartUserInfo actor, WebPartSettingList settingList)
        {
            return new WebPartText();
        }

        public override WebPartText RenderTitle(WebPartUserInfo actor)
        {
            string webpartText = string.Empty;

            if (_configSettingList.Exist("Title"))
            {
                webpartText = _configSettingList.Get("Title").Value;
            }

            return new WebPartText(webpartText);
        }

        public override WebPartHtml RenderSettings(WebPartUserInfo actor, WebPartSettingList settingList)
        {
            List<WebPartHtml> htmlCollection = new List<WebPartHtml>(); //傳入值宣告

            //SettingsHelper宣告
            SettingsHelper settingsHelper = new SettingsHelper(CategoryName);

            if (settingList.Exist("TestItemSetting")) //取得內建控制項key值，設計成需求內容
            {

                WebPartHtmlElementList elementList = new WebPartHtmlElementList();

                elementList.Add("第一項", "A");
                elementList.Add("第二項", "B");
                elementList.Add("第三項", "C");
                elementList.Add("第四項", "D");

                htmlCollection.Add(settingsHelper.CreateDropDownList("項目", elementList, settingList.Get("TestItemSetting")));

            }

            if (settingList.Exist("TestBoxSetting")) //取得自訂控制項key值與內容
            {
                htmlCollection.Add(settingsHelper.CreateTextBox("測試輸入框", settingList.Get("TestBoxSetting")));
            }

            return WebPartHtml.Aggregate(htmlCollection);
        }

    }
}
