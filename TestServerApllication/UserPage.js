angular.module("myapp", ['ngMaterial'])
    .controller("AppCtrl", function ($scope, $http, $sce, $timeout, $interval, $mdDialog) {
        $scope.cards = [];
        $scope.loading = true;
        $scope.totalHeadlines = 0;
        $scope.showModal = function (card) {
            $mdDialog.show({
                controller: DialogController,
                template: '<md-dialog aria-label="Title">' +
  '<form ng-cloak>' +
  '  <md-toolbar>' +
  '    <div class="md-toolbar-tools">' +
  '      <h2>Full Content of News</h2>' +
  '      <span flex></span>' +
  '      <md-button class="md-icon-button" ng-click="cancel()">' +
  '        <md-icon aria-label="Close dialog">clear</md-icon>' +
  '      </md-button>' +
  '    </div>' +
  '  </md-toolbar>' +
  '  <md-dialog-content>' +
  '    <div class="md-dialog-content">' +
  '      <h2>' + card.Headline + '</h2>' +
  card.NewsContent +
  '    </div>' +
  '  </md-dialog-content>' +
  '</form>' +
'</md-dialog>',
                parent: angular.element(document.body),
                clickOutsideToClose: true,
                fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
            })
            .then(function (answer) {
                $scope.status = 'You said the information was "' + answer + '".';
            }, function () {
                $scope.status = 'You cancelled the dialog.';
            });
        }
        $scope.URL = "http://economictimes.indiatimes.com";
        $scope.myContent = "";
        $scope.getNews = function () {

            $scope.loading = true;
            $http({
                method: 'Post',
                url: '/api/News/selectAll'
            }).then(function successCallback(response) {

                $scope.totalHeadlines = response.data.length;
                $scope.searchAndPush(response.data);
                $timeout(function () {
                    $scope.loading = false;
                }, 400);
            }, function errorCallback(response) {
                alert(JSON.stringify(response));
            });
        }

        $scope.searchAndPush = function (values) {
            angular.forEach(values, function (value, key) {
                value.Headline = $sce.trustAsHtml(value.Headline);
                value.NewsContent = $sce.trustAsHtml(value.NewsContent);
                value.DateTimeUpdate = moment(value.DateTimeUpdate).format("DD MMM, YYYY hh:mm a");
            });
            $scope.cards = values;
        }

        $scope.goToLink = function (URL) {
            window.location.href = URL;
        }
        $scope.firstCall = $scope.getNews();
        $interval(function () {
            $scope.getNews();
        }, 10000);

        function DialogController($scope, $mdDialog) {

            $scope.cancel = function () {
                $mdDialog.cancel();
            };
        }
    });
