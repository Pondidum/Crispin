import React from 'react'
import Toggle from '../Toggle'

const Dashboard = ({toggles}) => (
<div className="row">
    <ul>
        {toggles.map((toggle, index) => (<Toggle key={index} toggle={toggle} />)) }
    </ul>
</div>)

export default Dashboard
