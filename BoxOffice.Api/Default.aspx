<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BoxOffice.Api.Default" %>
<%@ Import Namespace="System.IO" %>
<%= File.ReadAllText(Server.MapPath("~/index.partial")) %>