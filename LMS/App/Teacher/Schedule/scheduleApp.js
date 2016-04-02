(function () {
    var app = angular.module('schedule', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Schedule/Views/scheduleIndexView.html',
                controller: 'indexCtrl'
            })
            .otherwise({
                redirectTo: '/'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    var monthNames = [
        'Januari',
        'Februari',
        'Mars',
        'April',
        'Maj',
        'Juni',
        'Juli',
        'Augusti',
        'September',
        'Oktober',
        'November',
        'December'
    ];

    app.controller('indexCtrl', ['$scope', '$http', function ($scope, $http) {
        boxes = [];
        for (var i = 1; i <= 35; ++i)
            boxes.push(i);
        $scope.boxes = boxes;

        $scope.curMonth = new Date().getMonth();

        var now = new Date();
        var firstday = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.numdays = new Date(now.getFullYear(), now.getMonth()+1, 0).getDate();
        $scope.firstday = firstday.getDay();
        $scope.monthName = monthNames[now.getMonth()];

        var cal = angular.element('#cal').children();
        $scope.cal = cal.html();

        $scope.foo = function (w, d) {
            var i = (w * 7) + d - $scope.firstday;
            if (i < 0 || i >= $scope.numdays)
                return '-';
            return i+1;
        };

        $scope.addMonth = function (delta) {
        };
    }]);
}());