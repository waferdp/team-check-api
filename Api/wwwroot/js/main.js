var  PopulateData = new Vue({
    el: '#performance-checklist',
    data: function() {
        return {
            result: [],
            form : [],
            errors: [],
            pagination: {
                currentPage : 1,
                totalPages: function () {
                    return !this.hasData() ? 0 : Math.floor(result.length / this.itemsPerPage) + 1; 
                },
                itemsPerPage: 5
            },
            loading: false,
        }
    },
    created: function() {
        this.fetchJsonData()
        .then(json => {
            this.result = json.map((e, i) => {
                return {  
                    index: i,
                    key: e,
                    value: null
                }
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
        getPage() {
            var end = this.pagination.currentPage * this.pagination.itemsPerPage;
            var start = end - this.pagination.itemsPerPage;
            var items = this.result.slice(start, end)
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
            loading = true;
            return fetch('/content/performance_checklist.json')
            .then(res => {
                return res.json();
            })
            .then(json => {
                loading=false;
                return json;
            });
        },
        validateForm() {
            var errors = [];
            this.result.forEach(item => {
                if(!item.value) {
                    errors.push(`Question ${item.index + 1} (page ${ Math.floor(item.index / this.pagination.itemsPerPage) + 1}) is not answered`)
                }
            });
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