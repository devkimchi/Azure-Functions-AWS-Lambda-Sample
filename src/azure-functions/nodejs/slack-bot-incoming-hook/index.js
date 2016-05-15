'use strict';
let https = require('https');

module.exports = function(context, req) {
    context.log('Node.js HTTP trigger function processed a request. RequestUri=%s', req.originalUrl);
    context.log(JSON.stringify(req, null, 2));

    if (req.query.name || (req.body/* && req.body.name*/)) {
        // Sets request options
        var options = {
            host: 'hooks.slack.com',
            port: '443',
            path: '/services/[WEBHOOK_KEY_GOES_HERE]',
            method: 'POST',
            headers: {
            'Content-Type': 'application/json'
            }
        };

        // Sets the request body    
        var data = {
            "text": "New push has come up by " + req.body.head_commit.author.name + " with commit Id of " + req.body.head_commit.id
        };

        const request = https.request(options, (res) => {
            let body = '';
            context.log('Status:', res.statusCode);
            context.log('Headers:', JSON.stringify(res.headers));
            res.setEncoding('utf8');
            res.on('data', (chunk) => body += chunk);
            res.on('end', () => {
                context.log('Successfully processed HTTPS response');
                // If we know it's JSON, parse it
                if (res.headers['content-type'] === 'application/json') {
                    body = JSON.parse(body);
                }
            });
        });
        request.on('error', (ex) => {
            context.log(ex);
        });
        request.write(JSON.stringify(data));
        request.end();

        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "Hello World"// + (req.query.name || req.body.name)
        };
    }
    else {
        context.res = {
            status: 400,
            body: "Please pass a name on the query string or in the request body"
        };
    }
    context.done();
};
