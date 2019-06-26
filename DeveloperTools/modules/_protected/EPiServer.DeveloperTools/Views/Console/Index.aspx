<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeveloperTools.Models.ConsoleModel>" MasterPageFile="../Shared/DeveloperTools.Master" %>
<%@ Import Namespace="EPiServer.DataAbstraction.RuntimeModel" %>

 <asp:Content ID="Content" runat="server" ContentPlaceHolderID="MainRegion">
    <script type="text/javascript" language="javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js"></script>
     <style>


.console {
  font-family: 'Fira Mono';
  width: 80%;
  height: 450px;
  box-sizing: border-box;
  /*margin: auto;*/
}

.console header {
  border-top-left-radius: 15px;
  border-top-right-radius: 15px;
  background-color: #555;
  height: 45px;
  line-height: 45px;
  text-align: center;
  color: #DDD;
}

.console .consolebody {
  border-bottom-left-radius: 15px;
  border-bottom-right-radius: 15px;
  box-sizing: border-box;
  padding: 20px;
  height: calc(100% - 40px);
  overflow: scroll;
  background-color: #000;
  color: #63de00;
}

.console .consolebody p {
  line-height: 1rem;
}
input:focus, textarea {
    outline: none !important;
}
#command{
    background-color:#000;color:#63de00;
    outline:none !important;
    border: 1px solid transparent;
    width:80%;
}
     </style>

    <h1>Developer Console</h1>
    <p>Unleash the power!</p>

    <div class="console">
    <header>
    <p>Admin</p>
  </header>
  <div class="consolebody">
    <p>type 'help' to see a list of commands</p>
    <p id="log"></p>
    <p>><input type="text" id="command" /></p>
  </div>
</div>
     

   <script>
       var cmdhistory = [];
       $("#command").on('keyup', function (e) {
           if (e.keyCode == 13) {

               var cmd = $('#command').val();
               cmdhistory.push(cmd);
               $('#log').append(cmd + '<br/>');
               $('#command').val('');
               $.post('<%=EPiServer.Shell.Paths.ToResource("EPiServer.DeveloperTools","Console/RunCommand/") %>', { command: cmd });
           } else if (e.keyCode == 38) {
               //Arrow up - if val is empty fill in last typed command. Otherwise it may be navigating in autocomplete?
               $('#command').val(cmdhistory[cmdhistory.length - 1]);
               e.stopPropagation();
               e.preventDefault();  
               e.returnValue = false;
               e.cancelBubble = true;
               return false;
           }
       });
       $(function () {
           $('#command').focus();
       });
       var last = 0;
       function RefreshLog() {
           $.getJSON('<%=EPiServer.Shell.Paths.ToResource("EPiServer.DeveloperTools","Console/FetchLog/") %>', { LastLogNo: last }, function (data) {
               last = data.LastNo;
               $.each(data.LogItems, function (idx, val) {
                   $('#log').append(val+'<br/>');
               })
               //LogItems[Time,Sender,Text]
               //$('#log').scrollTo('#command');
               window.setTimeout(RefreshLog, 1000);
           });
       }
       RefreshLog();
   </script>

</asp:Content>
