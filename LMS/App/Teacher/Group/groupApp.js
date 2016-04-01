(function () {
    var app = angular.module('group', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupIndexView.html',
                controller: 'groupCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupCreateView.html',
                controller: 'groupCtrl'
            })
            .when('/Edit/:id', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupEditView.html',
                controller: 'editCtrl'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('groupCtrl', ['$scope', 'dataService', function ($scope, dataService) {
        function getGroups() {
            dataService.get('Group/List', function (data) {
                $scope.groups = data;
            });
        };

        getGroups();
    }]);

    //app.directive('clearSelect', ['$parse', function($parse) {
    //    return {
    //        restrict: 'A',
    //        link: function (scope, element, attrs) {
    //            var expr = $parse(attrs.clearSelect);
    //            element.bind('click', function () {
    //                scope.$apply(function () {
    //                    expr(scope, {});
    //                });
    //            });
    //        }
    //    };
    //}]);

    app.controller('editCtrl', ['$scope', '$routeParams', 'dataService', '$compile', '$http', function ($scope, $routeParams, dataService, $compile, $http) {
        function getDetails() {
            var id = parseInt($routeParams['id']);
            dataService.get('Group/Details/' + id, function (data) {
                $scope.details = data;
                $scope.groupId = id;
            });
            dataService.get('Group/FreeStudents', function (data) {
                $scope.freeStudents = data;
            });
        }

        $scope.clearList = function (id) {
            var opts = angular.element(id)[0];
            for (var i = 0; i < opts.length; ++i) {
                opts[i].selected = false;
            }
            $scope.msg = 'clearing: ' + id;
        };

        $scope.transfer = function (fromId, toId) {
            var from = angular.element(fromId)[0];
            var to = angular.element(toId);

            var fromSel = from.selectedOptions
            var i;
            while ((i = fromSel.length) > 0) {
                var sel = fromSel[i - 1];
                var opt = angular.element('<option value="' + sel.value + '" ng-click="clearList(\'' + fromId + '\')">' + sel.innerHTML + '</option>');
                to.append(opt);
                $compile(opt)($scope);
                sel.remove();
            }
        };

        $scope.save = function () {
            var usedStudents = angular.element('#used').children();
            var freeStudents = angular.element('#free').children();
            var used = [];
            var free = [];
            for (var i = 0; i < usedStudents.length; ++i) {
                used.push(parseInt(usedStudents[i].value));
            }
            for (var i = 0; i < freeStudents.length; ++i) {
                free.push(parseInt(freeStudents[i].value));
            }

            var data = {
                used: used,
                free: free
            };

            $http.put(LMS.rootPath + 'Group/Update/' + $scope.groupId, data).then(function (resp) {
                $scope.msg = 'success';
            }, function (resp) {
                $scope.msg = 'failed to update group: ' +resp.status + ' ' + resp.statusText;
            });
        };

        getDetails();
    }]);
}());