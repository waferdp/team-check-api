var  PopulateData = new Vue({
    el: '#performance-checklist',
    data: function() {
        return {
            result: ["not loaded yet"],
            form : [],
            loading: false,
            hasData: false
        }
    },
    created: function() {
        this.fetchJsonData()
        .then(json => {
            this.result = json;
        });
    },
    methods: {
        fetchJsonData() {
            loading = true;
            return fetch('http://localhost:5000/content/performance_checklist.json')
            .then(res => {
                return res.json();
            })
            .then(json => {
                loading=false;
                hasData = json.length > 0;
                return json;
            });
        },
        sendJsonData() {
            var items = []
            for(var i = 0; i < this.form.length; i++) {
                var key = this.result[i].replace(".", "");
                var value = this.form[i];
                if(key && value) {
                    var item = {
                        'Index': i,
                        'Key': key, 
                        'Value': value
                    };
                    items.push(item);
                }
            }
            console.log(JSON.stringify(items));
            fetch('http://localhost:5000/api/performance-checklist', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(items)
            })
            .then(res =>{
                alert('Everything went well ');
                console.warn(res);
            });
        }
    }
});