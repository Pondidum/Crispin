import { fetch, addTask } from "domain-task";

export const listAllToggles = () => (dispatch, getState) => {
  let fetchTask = fetch(`/toggles`)
    .then(response => response.json())
    .then(data =>
      dispatch({
        type: "LIST_TOGGLES_SUCCESS",
        toggles: data
      })
    );

  addTask(fetchTask);
  dispatch({ type: "LIST_TOGGLES_REQUEST" });
};
