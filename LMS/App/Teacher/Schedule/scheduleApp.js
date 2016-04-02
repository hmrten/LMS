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

    function parseMSDate(s) {
        if (!s) return null;
        return new Date(parseInt(s.substr(6)));
    };

    app.directive('schedule', function () {
        return function (scope, el, attr) {
            if (scope.$last) {
                //alert('im the last: ', el.html());
            }
        };
    });

    app.controller('indexCtrl', ['$scope', '$http', function ($scope, $http) {
        function init() {
            $scope.curMonth = new Date().getMonth();
            genCal($scope.curMonth);
            getData();
        };

        function genCal(month) {
            var now = new Date(2016, month, 1);
            var firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
            $scope.numDays = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
            $scope.firstDay = firstDay.getDay();
            $scope.monthName = monthNames[now.getMonth()];
        };

        function getData() {
            $http.get(LMS.rootPath + 'Data/Schedule').then(function (resp) {
                var schedules = resp.data;

                var cal = angular.element('#cal');
                var trs = cal.children();

                for (var si = 0; si < schedules.length; ++si) {
                    var sdate = parseMSDate(schedules[si].date_end);
                    for (var i = 0; i < trs.length; ++i) {
                        var tds = trs[i];
                        for (var j = 0; j < tds.length; ++j) {
                            var str = tds[j].innerHTML;

                        }
                    }
                }

                //genCal($scope.curMonth);
            });
        };

        function inMonth(d) { return d > 0 && d <= $scope.numDays };
        function calcDay(i) { return i - $scope.firstDay + 1 };

        $scope.sched = {
            css: function (i) {
                var d = calcDay(i);
                if (!inMonth(d))
                    return '';
                if (d == 10)
                    return 'foo';
            },
            html: function (i) {
                var d = calcDay(i);
                if (!inMonth(d))
                    return '-';

                return d;
            },
            click: function (i) {
                var d = calcDay(i);
                if (inMonth(d)) {
                    alert('you clicked on day: ' + d);
                }
            }
        };

        $scope.addMonth = function (delta) {
            $scope.curMonth += delta;
            genCal($scope.curMonth);
        };

        init();
    }]);
}());