import { fetch, addTask } from "domain-task";

export const createToggle = (name, description) => (dispatch, getState) => {
  const options = {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      name: name,
      description: description
    })
  };

  let fetchTask = fetch(`/toggles`, options).then(response =>
    dispatch({ type: "CREATE_TOGGLE_SUCCESS" })
  );

  addTask(fetchTask);
  dispatch({ type: "CREATE_TOGGLE_REQUEST" });
};
