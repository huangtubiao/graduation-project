//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyUniversity.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Answer = new HashSet<Answer>();
            this.Article = new HashSet<Article>();
            this.ArticleComment = new HashSet<ArticleComment>();
            this.Clocklog = new HashSet<Clocklog>();
            this.Contact = new HashSet<Contact>();
            this.Contact1 = new HashSet<Contact>();
            this.Follows = new HashSet<Follows>();
            this.Follows1 = new HashSet<Follows>();
            this.LastChat = new HashSet<LastChat>();
            this.LastChat1 = new HashSet<LastChat>();
            this.Love1 = new HashSet<Love>();
            this.Message = new HashSet<Message>();
            this.Message1 = new HashSet<Message>();
            this.Plan = new HashSet<Plan>();
            this.PlanComeOn = new HashSet<PlanComeOn>();
            this.Plancomment = new HashSet<Plancomment>();
            this.ProjectRecord = new HashSet<ProjectRecord>();
            this.ProjectRecord1 = new HashSet<ProjectRecord>();
            this.Question = new HashSet<Question>();
            this.Question1 = new HashSet<Question>();
            this.RecentVisitor = new HashSet<RecentVisitor>();
            this.RecentVisitor1 = new HashSet<RecentVisitor>();
            this.RecoverAnswer = new HashSet<RecoverAnswer>();
            this.RecoverAnswer1 = new HashSet<RecoverAnswer>();
            this.Supervice = new HashSet<Supervice>();
            this.SuperiorLetter = new HashSet<SuperiorLetter>();
            this.SuperiorLetter1 = new HashSet<SuperiorLetter>();
        }
    
        public long userId { get; set; }
        public string userName { get; set; }
        public string userRealName { get; set; }
        public string userImg { get; set; }
        public string userPaw { get; set; }
        public string userAccount { get; set; }
        public string userIntro { get; set; }
        public Nullable<long> userRank { get; set; }
        public string userSex { get; set; }
        public string userLove { get; set; }
        public string userSchool { get; set; }
        public string userDepart { get; set; }
        public string userStartYear { get; set; }
        public string userClass { get; set; }
        public string userDorm { get; set; }
        public string userMiddleSchool { get; set; }
        public string userMerit { get; set; }
        public Nullable<int> userAnswerNum { get; set; }
        public Nullable<int> userLoved { get; set; }
        public string userDream { get; set; }
        public Nullable<int> planFinishNum { get; set; }
        public Nullable<int> userVisitedNum { get; set; }
        public Nullable<int> userWeekExps { get; set; }
        public Nullable<System.DateTime> userWeekExpsStartTime { get; set; }
        public string userMeritIntro { get; set; }
        public Nullable<int> userFans { get; set; }
        public Nullable<int> userProjectNum { get; set; }
        public Nullable<int> userVitality { get; set; }
    
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<Article> Article { get; set; }
        public virtual ICollection<ArticleComment> ArticleComment { get; set; }
        public virtual ICollection<Clocklog> Clocklog { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<Contact> Contact1 { get; set; }
        public virtual ICollection<Follows> Follows { get; set; }
        public virtual ICollection<Follows> Follows1 { get; set; }
        public virtual ICollection<LastChat> LastChat { get; set; }
        public virtual ICollection<LastChat> LastChat1 { get; set; }
        public virtual Love Love { get; set; }
        public virtual ICollection<Love> Love1 { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Message> Message1 { get; set; }
        public virtual ICollection<Plan> Plan { get; set; }
        public virtual ICollection<PlanComeOn> PlanComeOn { get; set; }
        public virtual ICollection<Plancomment> Plancomment { get; set; }
        public virtual ICollection<ProjectRecord> ProjectRecord { get; set; }
        public virtual ICollection<ProjectRecord> ProjectRecord1 { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<Question> Question1 { get; set; }
        public virtual ICollection<RecentVisitor> RecentVisitor { get; set; }
        public virtual ICollection<RecentVisitor> RecentVisitor1 { get; set; }
        public virtual ICollection<RecoverAnswer> RecoverAnswer { get; set; }
        public virtual ICollection<RecoverAnswer> RecoverAnswer1 { get; set; }
        public virtual ICollection<Supervice> Supervice { get; set; }
        public virtual ICollection<SuperiorLetter> SuperiorLetter { get; set; }
        public virtual ICollection<SuperiorLetter> SuperiorLetter1 { get; set; }
    }
}
