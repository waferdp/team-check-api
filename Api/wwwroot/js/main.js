var PopulateData = new Vue({
    el: '#performance-checklist',
    data: function() {
        return {
            result: [],
            errors: [],
            pagination: {
                currentPage : 1,
                totalPages: function () {
                    return !this.hasData() ? 0 : Math.floor(this.result.length / this.itemsPerPage) + 1; 
                },
                itemsPerPage: 5
            },
            loading: false,
        };
    },
    created: function() {
        this.fetchJsonData()
        .then(json => {
            this.result = json.map((e, i) => {
                return {  
                    index: i,
                    key: e,
                    value: null
                };
            });
        });
    },
    computed: {
        hasData() {
            return this.result.length > 0;
        },
        isFirstPage() {
            return this.pagination.currentPage <= 1;
        },
        isLastPage() {
            return this.pagination.currentPage * this.pagination.itemsPerPage >= this.result.length;
        },
    },
    methods: {
        getPage(){ 
            var end = this.pagination.currentPage * this.pagination.itemsPerPage;
            var start = end - this.pagination.itemsPerPage;
            var items = this.result.slice(start, end);
            return  {
                pagination: this.pagination,
                items: items
            };
        },
        nextPage() {
            this.pagination.currentPage++;
        },
        prevPage() {
            this.pagination.currentPage--;
        },
        fetchJsonData() {
            this.loading = true;
            return fetch('/content/performance_checklist.json')
            .then(res => {
                return res.json();
            })
            .then(json => {
                this.loading=false;
                return json;
            });
        },
        validateForm() {
            var validationErrors = [];
            this.result.forEach(item => {
                if(!item.value) {
                    let newError = {
                        question: item.index,
                        page: Math.floor(item.index / this.pagination.itemsPerPage)
                    };
                    validationErrors.push(newError);
                }
            });
            var errors = [];
            var totalPages = Math.floor(this.result.length / this.pagination.itemsPerPage);
            for (var i = 0; i < totalPages; i++) {
                var errorsOnPage = validationErrors.filter(error => error.page === i);
                if(errorsOnPage) {
                    errors.push(`Question${errorsOnPage.length > 1 ? 's' : ''} ${errorsOnPage.map(error => error.question + 1)} on page ${i+1} ${errorsOnPage.length > 1 ? 'are' : 'is'} not answered`);
                }
            }
            this.errors = errors;
        },
        sendJsonData() {
            this.validateForm();

            if(this.errors.length) {
                return;
            }
            console.log(JSON.stringify(this.result));
            fetch('/api/performance-checklist', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.result)
            })
            .then(res =>{
                alert('Everything went well ');
                console.warn(res);
            });
        }
    }
});