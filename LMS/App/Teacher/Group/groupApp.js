(function () {
    var app = angular.module('group', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupIndexView.html',
                controller: 'indexCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupCreateView.html',
                controller: 'createCtrl'
            })
            .when('/Edit/:id', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupEditView.html',
                controller: 'editCtrl'
            })
            .otherwise({
                redirectTo: '/'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('indexCtrl', ['$scope', '$location', '$http', 'dataService', function ($scope, $location, $http, dataService) {
        function getGroups() {
            dataService.get('Group/List', function (data) {
                $scope.groups = data;
            });
        };

        $scope.delete = function (id) {
            $http.delete(LMS.rootPath + 'Group/Delete/' + id).then(function (resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Ta bort lyckades!',
                    text: resp.statusText
                };
                getGroups();
            }, function (resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Ta bort misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
            });
        };

        getGroups();
    }]);

    app.controller('createCtrl', ['$scope', 'dataService', function ($scope, dataService) {
        function getTeachers() {
            dataService.get('Data/Teachers', function (data) {
                $scope.teachers = data;
            });
        };

        $scope.create = function () {
            var data = {
                name: $scope.name,
                teacherId: $scope.teacherId
            };

            dataService.post('Group/Create', data, function (resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Skapa lyckades!',
                    text: resp.statusText
                };
            }, function (resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Skapa misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
            });
        };

        getTeachers();
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
                var details = data;
                dataService.get('Group/FreeStudents', function (data) {
                    var freeStudents = data;
                    dataService.get('Data/Teachers', function (data) {
                        $scope.freeStudents = freeStudents;
                        $scope.teachers = data;
                        $scope.details = details;
                        $scope.groupId = id;
                    });
                });
            });
        }

        $scope.clearList = function (id) {
            var opts = angular.element(id)[0];
            for (var i = 0; i < opts.length; ++i) {
                opts[i].selected = false;
            }
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
            $scope.msg = null;

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
                id: $scope.details.id,
                teacher_id: $scope.details.teacher_id,
                used: used,
                free: free
            };

            $http.put(LMS.rootPath + 'Group/Update/' + $scope.groupId, data).then(function (resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Ändring lyckades!',
                    text: resp.statusText
                };
            }, function (resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Ändring misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
            });
        };

        getDetails();
    }]);
}());