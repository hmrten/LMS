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
        function init() {
            $scope.curMonth = new Date().getMonth();
            genCal($scope.curMonth);
        };

        function genCal(month) {
            var now = new Date(2016, month, 1);
            var firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
            $scope.numDays = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
            $scope.firstDay = firstDay.getDay();
            $scope.monthName = monthNames[now.getMonth()];
        }

        $scope.sched = {
            css: function (i) {
                var d = i - $scope.firstDay;
                if (d < 0 || d >= $scope.numDays)
                    return '';
                ++d;
                if (d == 10)
                    return 'foo';
            },
            html: function (i) {
                var d = i - $scope.firstDay;
                if (d < 0 || d >= $scope.numDays)
                    return '-';
                return d + 1;
            }
        };

        $scope.addMonth = function (delta) {
            $scope.curMonth += delta;
            genCal($scope.curMonth);
        };

        init();
    }]);
}());