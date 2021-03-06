const { broadcast } = require("../Utils/Broadcast.js");
const { spawnPositions } = require("../Vars/SpawnPosition.js");
const { DefaultValue } = require("../Vars/DefaultPlayerValue.js");
const { DataVO } = require("../VO/DataVO.js");

// 유저가 접속했을 때 호출

function userConnectedHandler(socket)
{
    const payload = JSON.stringify({
        id: socket.sessionId,
        pos: spawnPositions[Math.round(Math.random() * spawnPositions.length) % spawnPositions.length],
        hp: DefaultValue.hp
        // TODO : 모든 유저의 위치 데이터를 전달해 주어야 함
        // TODO : HistoryHandler 비슷한 거 만들어야 함
    });

    console.log("UserConnectedHandler: " + payload);

    broadcast(JSON.stringify(new DataVO("connect", payload))); // 문자열로 바꿔줌
}

exports.userConnectedHandler = userConnectedHandler;