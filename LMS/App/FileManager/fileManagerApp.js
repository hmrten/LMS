(function () {
    var app = angular.module('fileManager', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	//$routeProvider
        //    .when('/', {
        //    	templateUrl: LMS.rootPath + 'App/FileManager/Views/fileManagerIndexView.html',
        //    	controller: 'fileManagerIndexCtrl'
        //    });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.directive('expandTree', ['$parse', '$compile', function ($parse, $compile) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.on('click', function () {
                    if (!element.hasClass('expanded')) {
                        element.addClass('expanded');
                        var dirs = angular.fromJson(attrs.expandTree).dirs;
                        var ul = angular.element('<ul></ul>');
                        dirs.forEach(function (e, i, a) {
                            var li = angular.element('<li></li>');
                            li.append('<a href="#" expand-tree=' + angular.toJson(e) + '>' + e.name + '</a>');
                            ul.append(li);
                            $compile(li)(scope);
                        });
                        element.parent().append(ul);
                    } else {
                        element.removeClass('expanded');
                        element.parent().children()[1].remove();
                    }
                });
            }
        };
    }]);

    app.controller('fileManagerCtrl', ['$scope', '$http', function ($scope, $http) {
        $http.get(LMS.rootPath + 'FileManager/List').then(
            function (resp) {
                $scope.dirs = resp.data.dirs;
            },
            function (resp) {
            });

        $scope.expand = function (dir) {

        };
    }]);
}());