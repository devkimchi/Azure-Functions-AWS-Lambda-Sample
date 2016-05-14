'use strict';
let https = require('https');

/**
 * Pass the data to send as `event.data`, and the request options as
 * `event.options`. For more information see the HTTPS module documentation
 * at https://nodejs.org/api/https.html.
 *
 * Will succeed with the response body.
 */
exports.handler = (event, context, callback) => {
    // Checks the event details
    console.log(JSON.stringify(event, null, 2));
    
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
        "text": "New push has come up by " + event.head_commit.author.name + " with commit Id of " + event.head_commit.id
    };

    const req = https.request(options, (res) => {
        let body = '';
        console.log('Status:', res.statusCode);
        console.log('Headers:', JSON.stringify(res.headers));
        res.setEncoding('utf8');
        res.on('data', (chunk) => body += chunk);
        res.on('end', () => {
            console.log('Successfully processed HTTPS response');
            // If we know it's JSON, parse it
            if (res.headers['content-type'] === 'application/json') {
                body = JSON.parse(body);
            }
            callback(null, body);
        });
    });
    req.on('error', callback);
    req.write(JSON.stringify(data));
    req.end();
};
