<h3>Success</h3>
<p>Thank you for your submission!</p>
<?php

/* Requires GuzzleHTTP Library */
/*
/* Install: composer require guzzlehttp
/*
*/

require_once('sites/all/libraries/composer/autoload.php');

// Defined Constants
$tenantId = '{client_id}';
$clientId = '{client_id}';
$clientSecret = '{client_secret}';
$url = 'https://login.microsoftonline.com/' . $tenantId . '/oauth2/token?api-version=1.0';

// Get Form Data
$sid = intval($_GET['sid']);
$data = webform_get_submissions(array('sid' => $sid));
$data = $data[$sid]->data;

$fname = isset($data[2][0]) ? $data[2][0] : "";
$organ = isset($data[3][0]) ? $data[3][0] : "";
$phone = isset($data[4][0]) ? $data[4][0] : "";
$email = isset($data[5][0]) ? $data[5][0] : "";
$messg = isset($data[6][0]) ? $data[6][0] : "";

// Get Access Token
$guzzle = new \GuzzleHttp\Client();
$token = json_decode($guzzle->post($url, [
    'form_params' => [
        'client_id' => $clientId,
        'client_secret' => $clientSecret,
        'resource' => 'https://graph.microsoft.com/',
        'grant_type' => 'client_credentials',
    ],
])->getBody()->getContents());
$accessToken = $token->access_token;

// Send Data as POST request to Sharepoint via Microsoft Graph API
$post_url = 'https://graph.microsoft.com/v1.0/sites/{tenantid},c0cefe40-beeb-41a9-b4f5-9960bcfa010b,fbb78c64-1220-42fe-a319-94c493a9a105/lists/6f53c37b-d6ba-46c3-91a9-2e942a984af9/items';
$guzzle2 = new \GuzzleHttp\Client([
    'headers' => [ 'Content-Type' => 'application/json', 'Authorization' => 'Bearer ' . $accessToken ]
]);
try {
    $newItem = $guzzle2->request('POST', $post_url, [
        'json' => [ 'fields' => [
            'Title' => $fname,
            'Organization' => $organ,
            'Phone' => $phone,
            'Email' => $email,
            'Message' => $messg,
            'Source' => 'sharepointOnline-GraphAPI-DrupalSTG' ],
        ],
    ]);
} catch (GuzzleHttp\Exception\ClientException $e) {
    // Catch any exceptions Guzzle throws
    $response = $e->getResponse();
    $responseBodyAsString = $response->getBody()->getContents();
    error_log('SBO Webform item failed: '.$responseBodyAsString);
    exit();
}

unset($guzzle);
unset($guzzle2);

?>