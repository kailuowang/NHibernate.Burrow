using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step06c : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BurrowFramework bf = new BurrowFramework();
            IConversation conversation = bf.CurrentConversation;

            if (conversation == null)
            {
                throw new Exception("The page doesn't have conversation");
            }

            object lastConversationId = Session["conversationId"];
            if (lastConversationId == null)
            {
                throw new Exception("We haven't found the Id of previous conversation");
            }

            if (conversation.Id.Equals(lastConversationId))
            {
                throw new Exception("The conversation is same that previous, the new conversation was not created");
            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Session["continue_a"] = true;

        hdClose.Value = "1";
    }
}