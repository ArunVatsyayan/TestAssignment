<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="TestServerApllication.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/angular_material/1.1.0/angular-material.min.css"/>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"/>
</head>
<body>
    <div ng-app="myapp" ng-controller="AppCtrl">
        <div layout="row">
             <md-card flex>
        <md-card-title>
          <md-card-title-text>
              <div layout="row" layout-align="center center">
            <span class="md-headline">Top Headlines</span>
                
              </div>  
              <div layout="row" layout-sm="column" layout-align="space-around" ng-show="loading">
      <md-progress-circular md-mode="indeterminate"></md-progress-circular>
    </div>
          </md-card-title-text>
        </md-card-title>
      </md-card>
        </div>
        <div layout="row"  ng-repeat="card in cards" >
            <md-card flex>
        <md-card-title>
          <md-card-title-text>
            <span class="md-headline" ng-bind-html="card.Headline"></span>
          </md-card-title-text>
        </md-card-title>
        <md-card-content ng-bind-html="card.NewsContent" style="max-height:147px; overflow:hidden">
          <%--{{card.NewsContent}}--%>
        </md-card-content>
        <md-card-actions >
            <div layout="row" >
                <md-button>{{card.DateTimeUpdate}}</md-button>
            </div>
            <div layout="row" layout-align="end center">
          <md-button ng-click="showModal(card)">Show Full News</md-button>
          <md-button ng-click="goToLink(card.FullNewsLink)">Go To Link</md-button>
                </div>
        </md-card-actions>
      </md-card>
        </div>
    </div>

    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.5.5/angular.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.5.5/angular-animate.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.5.5/angular-aria.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.5.5/angular-messages.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/angular_material/1.1.0/angular-material.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.1/moment.js"></script>
    <script src="/UserPage.js"></script>
</body>
</html>
