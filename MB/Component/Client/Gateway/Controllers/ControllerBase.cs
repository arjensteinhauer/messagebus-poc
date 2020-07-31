//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.6.2.0 (NJsonSchema v10.1.23.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."

namespace MB.Client.Gateway.Service.Controllers
{
    using System = global::System;
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.6.2.0 (NJsonSchema v10.1.23.0 (Newtonsoft.Json v12.0.0.0))")]
    public abstract class EventsControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>I am alive event</summary>
        /// <param name="body">Message for I am alive event</param>
        /// <returns>String result of I am alive event publish</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("events/iAmAlive")]
        public abstract System.Threading.Tasks.Task<string> IAmAlive([Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] IAmAliveRequest body);
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.6.2.0 (NJsonSchema v10.1.23.0 (Newtonsoft.Json v12.0.0.0))")]
    public abstract class Message1ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>Message1 echo call</summary>
        /// <param name="body">Input for echo callchain</param>
        /// <returns>String result of echo callchain</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("message1/echo")]
        public abstract System.Threading.Tasks.Task<string> Echo([Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] EchoRequest body);
    
        /// <summary>Message1 request response call</summary>
        /// <param name="body">Input for request response</param>
        /// <returns>String result of request response</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("message1/requestResponse")]
        public abstract System.Threading.Tasks.Task<string> RequestResponse([Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] RequestResponseRequest body);
    
        /// <summary>Message1 oneway call</summary>
        /// <param name="body">Input for oneway</param>
        /// <returns>String result of oneway</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("message1/oneWay")]
        public abstract System.Threading.Tasks.Task<string> OneWay([Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] OneWayRequest body);
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.6.2.0 (NJsonSchema v10.1.23.0 (Newtonsoft.Json v12.0.0.0))")]
    public abstract class Message2ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>Message2 echo call</summary>
        /// <param name="body">Input for echo callchain</param>
        /// <returns>String result of echo callchain</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("message2/echo")]
        public abstract System.Threading.Tasks.Task<string> Echo([Microsoft.AspNetCore.Mvc.FromBody] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] EchoRequest body);
    
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.23.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class IAmAliveRequest 
    {
        [Newtonsoft.Json.JsonProperty("input", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Input { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.23.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class EchoRequest 
    {
        [Newtonsoft.Json.JsonProperty("input", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Input { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.23.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class RequestResponseRequest 
    {
        [Newtonsoft.Json.JsonProperty("input", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Input { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.23.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class OneWayRequest 
    {
        [Newtonsoft.Json.JsonProperty("input", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Input { get; set; }
    
    
    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108