import React from "react";

const Conditions = () => (
  <div>
    <ul>
      <li>
        When user is in group <b>Alpha Testers</b>
      </li>
      <li>
        When user is in group <b>Beta Testers</b>
      </li>
      <li>
        When All are True
        <ul>
          <li>
            When user is in group <b>Canary</b>
          </li>
          <li>
            When user is in <b>5%</b> of users
          </li>
        </ul>
      </li>
    </ul>
  </div>
);

export default Conditions;
