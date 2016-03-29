(function () {
    var app = angular.module('fileManager');

    app.directive('expandTree', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.on('click', function () {
                    if (!element.hasClass('expanded')) {
                        element.addClass('expanded');
                        var name = attrs.expandTree;
                        var ul = angular.element('<ul></ul>');
                        ul.append(angular.element('<li>' + name + ': test 1</li>'));
                        ul.append(angular.element('<li>' + name + ': test 2</li>'));
                        ul.append(angular.element('<li>' + name + ': test 3</li>'));
                        element.parent().append(ul);
                    } else {
                        element.removeClass('expanded');
                        element.parent().remove('ul');
                    }
                });
            }
        };
    }]);

    app.controller('fileManagerIndexCtrl', ['$scope', '$http', function ($scope, $http) {
        $http.get(LMS.rootPath + 'FileManager/List').then(
            function (resp) {
                $scope.dirs = resp.data.children;
            },
            function (resp) {
            });

        $scope.expand = function (dir) {

        };
    }]);
}());